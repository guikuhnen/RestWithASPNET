using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithASPNET.Model.Base
{
	public abstract	class BaseEntity
	{
		[Key]
		[Column("id")]
		public long Id { get; set; }
	}
}
