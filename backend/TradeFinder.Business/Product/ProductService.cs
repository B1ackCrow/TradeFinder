using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SkiaSharp;
using TradeFinder.Contracts.Product;
using TradeFinder.DataManager;

namespace TradeFinder.Business.Product;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IWebHostEnvironment _environment;

    public ProductService(IProductRepository productRepository, IWebHostEnvironment environment)
    {
        _productRepository = productRepository;
        _environment = environment;
    }

    public async Task UpdateProduct(
        Guid id,
        string name,
        IFormFile? image = null,
        Guid? parentProductId = null)
    {
        DataManager.Product? product = await _productRepository.GetById(id);

        if (product == null)
        {
            throw new ArgumentException("Продукт не найден");
        }

        product.Name = name;
        product.ParentProductId = parentProductId;

        if (image != null)
        {
            string? oldImagePath = product.Image;

            string newImagePath = await SaveProductImage(image);

            product.Image = newImagePath;

            await _productRepository.Update(product);

            if (!string.IsNullOrWhiteSpace(oldImagePath))
            {
                await DeleteProductImage(oldImagePath);
            }

            return;
        }

        await _productRepository.Update(product);
    }

    public async Task<Guid> CreateProduct(string name, IFormFile? image = null, Guid? parentProductId = null)
    {
        bool exists = await _productRepository.ExistsByName(name);

        if (exists)
        {
            throw new ArgumentException($"Продукт с таким именем уже существует");
        }

        string? imagePath = null;

        try
        {
            if (image != null)
            {
                imagePath = await SaveProductImage(image);
            }

            DataManager.Product product = new DataManager.Product()
            {
                Name = name,
                Image = imagePath,
                ParentProductId = parentProductId
            };

            Guid productId = await _productRepository.Create(product);

            return productId;
        }
        catch (Exception ex)
        {
            if (imagePath != null)
            {
                await DeleteProductImage(imagePath);
            }

            throw;
        }
    }

    public async Task<GetProductByIdResponse> GetProductById(Guid id)
    {
        DataManager.Product? product = await _productRepository.GetById(id);

        if (product is null)
        {
            throw new InvalidOperationException("Не удалось получить категорию");
        }

        GetProductByIdResponse result = new()
        {
            Id = product.Id,
            Name = product.Name,
            Image = product.Image,
            ParentId = product.ParentProductId
        };

        return result;
    }

    private static void SortProducts(List<GetAllProductsResponse> products)
    {
        products.Sort((x, y) =>
            string.Compare(
                x.Name,
                y.Name,
                StringComparison.CurrentCultureIgnoreCase
            )
        );

        foreach (var product in products)
        {
            if (product.Children.Count > 0)
            {
                SortProducts(product.Children);
            }
        }
    }
    
    public async Task<List<GetAllProductsResponse>> GetAllProducts()
    {
        List<DataManager.Product> products = await _productRepository.GetAll();

        var productsDictionary = products.ToDictionary(
            x => x.Id,
            x => new GetAllProductsResponse
            {
                Id = x.Id,
                Name = x.Name,
                Children = new List<GetAllProductsResponse>()
            });

        var result = new List<GetAllProductsResponse>();

        foreach (var product in products)
        {
            var responseProduct = productsDictionary[product.Id];

            if (product.ParentProductId is null)
            {
                result.Add(responseProduct);
                continue;
            }

            if (productsDictionary.TryGetValue(product.ParentProductId.Value, out var parent))
            {
                parent.Children!.Add(responseProduct);
            }
        }

        SortProducts(result);
        
        return result;
    }

    private async Task<string> SaveProductImage(IFormFile image)
    {
        if (image.Length == 0)
        {
            throw new ArgumentException("Файл изображения пуст");
        }

        const long maxFileSize = 10 * 1024 * 1024;

        if (image.Length > maxFileSize)
        {
            throw new ArgumentException("Размер изображения не должен превышать 10 МБ");
        }

        if (!image.ContentType.StartsWith("image/"))
        {
            throw new ArgumentException("Загруженный файл не является изображением");
        }

        string uploadsDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads",
            "products"
        );

        Directory.CreateDirectory(uploadsDirectory);

        string fileName = $"{Guid.NewGuid():N}.webp";
        string fullPath = Path.Combine(uploadsDirectory, fileName);

        await using Stream inputStream = image.OpenReadStream();

        using SKBitmap originalBitmap = SKBitmap.Decode(inputStream);

        if (originalBitmap is null)
        {
            throw new ArgumentException("Не удалось прочитать изображение");
        }

        int width = originalBitmap.Width;
        int height = originalBitmap.Height;

        if (width <= 0 || height <= 0)
        {
            throw new ArgumentException("Некорректные размеры изображения");
        }

        const int targetSize = 600;

        double scale = Math.Max(
            (double)targetSize / width,
            (double)targetSize / height
        );

        int resizedWidth = (int)Math.Round(width * scale);
        int resizedHeight = (int)Math.Round(height * scale);

        using SKBitmap resizedBitmap = originalBitmap.Resize(
            new SKImageInfo(resizedWidth, resizedHeight),
            new SKSamplingOptions(
                SKFilterMode.Linear,
                SKMipmapMode.Linear
            )
        );

        if (resizedBitmap is null)
        {
            throw new Exception("Не удалось изменить размер изображения");
        }

        int cropX = (resizedWidth - targetSize) / 2;
        int cropY = (resizedHeight - targetSize) / 2;

        using SKBitmap croppedBitmap = new(
            targetSize,
            targetSize
        );

        using (SKCanvas canvas = new(croppedBitmap))
        {
            canvas.Clear(SKColors.White);

            canvas.DrawBitmap(
                resizedBitmap,
                new SKRect(
                    cropX,
                    cropY,
                    cropX + targetSize,
                    cropY + targetSize
                ),
                new SKRect(
                    0,
                    0,
                    targetSize,
                    targetSize
                )
            );
        }

        using SKImage skImage = SKImage.FromBitmap(croppedBitmap);

        using SKData data = skImage.Encode(
            SKEncodedImageFormat.Webp,
            80
        );

        await using FileStream fs = new(
            fullPath,
            FileMode.Create
        );

        data.SaveTo(fs);

        return $"/uploads/products/{fileName}";
    }

    private Task DeleteProductImage(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return Task.CompletedTask;
        }

        string fullPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            imagePath.TrimStart('/')
        );

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}