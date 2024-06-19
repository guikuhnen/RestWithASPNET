using RestWithASPNET.Model.Base;

namespace RestWithASPNET.Repository.Base
{
	public interface IBaseRepository<T> where T : BaseEntity
	{
		T Create(T obj);
		ICollection<T> FindAll();
		T FindById(long id);
		T Update(T obj);
		void Delete(long id);
		bool Exists(long id);
	}
}
