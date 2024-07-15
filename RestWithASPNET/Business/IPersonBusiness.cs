using RestWithASPNET.Data.VO;

namespace RestWithASPNET.Business
{
	public interface IPersonBusiness
    {
        PersonVO Create(PersonVO person);
        ICollection<PersonVO> FindAll();
		PersonVO? FindById(long id);
		PersonVO Update(PersonVO person);
        void Delete(long id);
		PersonVO? SetStatus(long id);
	}
}
