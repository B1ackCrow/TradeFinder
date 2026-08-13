using TradeFinder.Contracts.Product;
using TradeFinder.DataManager;

namespace TradeFinder.Business.Product;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Guid> CreateProduct(string name, string? image = null, Guid? parentProductId = null)
    {
        bool exists = await _productRepository.ExistsByName(name);
        if (exists)
        {
            throw new ArgumentException($"Продукт с таким именем уже существует");
        }
        
        DataManager.Product product = new DataManager.Product()
        {
            Name = name,
            Image = image,
            ParentProductId = parentProductId
        };
        
        Guid productId = await _productRepository.Create(product);
        
        return productId;
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

        return result;
    }
}