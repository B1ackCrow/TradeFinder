namespace TradeFinder.DataManager;

/// <summary>
/// Предоставляет методы для управления товарами.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Создает новый товар.
    /// </summary>
    /// <param name="product">Данные создаваемого товара.</param>
    /// <returns>Идентификатор созданного товара.</returns>
    Task<Guid> Create(Product product);

    /// <summary>
    /// Обновляет существующий товар.
    /// </summary>
    /// <param name="product">Новые данные товара.</param>
    Task Update(Product product);

    /// <summary>
    /// Получает товар по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор товара.</param>
    /// <returns>
    /// Экземпляр <see cref="Product"/>, если товар найден; иначе <see langword="null"/>.
    /// </returns>
    Task<Product?> GetById(Guid id);

    /// <summary>
    /// Получает список всех товаров.
    /// </summary>
    /// <returns>Коллекция товаров.</returns>
    Task<List<Product>> GetAll();

    /// <summary>
    /// Проверяет, существует ли товар с указанным названием.
    /// </summary>
    /// <param name="name">Название товара.</param>
    /// <returns>
    /// <see langword="true"/>, если товар существует; иначе <see langword="false"/>.
    /// </returns>
    Task<bool> ExistsByName(string name);

    /// <summary>
    /// Проверяет, существует ли товар с указанным названием,
    /// исключая товар с заданным идентификатором.
    /// Используется при обновлении товара.
    /// </summary>
    /// <param name="id">Идентификатор исключаемого товара.</param>
    /// <param name="name">Название товара.</param>
    /// <returns>
    /// <see langword="true"/>, если существует другой товар с таким названием;
    /// иначе <see langword="false"/>.
    /// </returns>
    Task<bool> ExistsByName(Guid id, string name);
}