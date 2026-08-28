namespace TradeFinder.DataManager;

public interface ICompanyRepository
{
    /// <summary>
    /// Возвращает список всех компаний.
    /// </summary>
    Task<List<Company>> GetAll();

    /// <summary>
    /// Возвращает компанию по её идентификатору.
    /// Если компания не найдена, возвращает null.
    /// </summary>
    Task<Company?> GetById(Guid companyId);

    /// <summary>
    /// Создаёт новую компанию и возвращает её идентификатор.
    /// </summary>
    Task<Guid> Create(Company company);

    /// <summary>
    /// Обновляет основные данные существующей компании.
    /// </summary>
    Task Update(Company company);

    /// <summary>
    /// Удаляет компанию по её идентификатору.
    /// </summary>
    Task Delete(Guid id);

    /// <summary>
    /// Добавляет продукт компании.
    /// </summary>
    Task CreateProduct(Guid companyId, Guid productId);

    /// <summary>
    /// Удаляет продукт у компании.
    /// </summary>
    Task DeleteProduct(Guid companyId, Guid productId);

    /// <summary>
    /// Добавляет контакт компании.
    /// </summary>
    Task CreateContact(
        Guid companyId,
        Guid companyContactTypeId,
        string value
    );

    /// <summary>
    /// Удаляет контакт компании по идентификатору контакта.
    /// </summary>
    Task DeleteContact(Guid companyContactId);

    /// <summary>
    /// Добавляет локацию компании.
    /// </summary>
    Task CreateLocation(Guid companyId, Guid locationId);

    /// <summary>
    /// Удаляет локацию у компании.
    /// </summary>
    Task DeleteLocation(Guid companyId, Guid locationId);
}