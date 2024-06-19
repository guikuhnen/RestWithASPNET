namespace RestWithASPNET.Data.Converter.Contract
{
	public interface IParser<O, D>
	{
		D Parse(O origin);
		ICollection<D> Parse(ICollection<O> origin);
	}
}
