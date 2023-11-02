using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Core.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {

        }

        public Guid Id { get; set; }
        public DateTime? DataCriacao { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(1000)]
        public string? UsuarioCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(1000)]
        public string? UsuarioAtualizacao { get; set; }

        public void NovaInsercao(string usuarioId)
        {
            DataCriacao = DateTime.Now;
            UsuarioCriacao = usuarioId;
        }

        public void NovaAtualizacao(string usuarioId)
        {
            DataAtualizacao = DateTime.Now;
            UsuarioAtualizacao = usuarioId;
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        public class EntityComparer<T> : IEqualityComparer<T> where T : EntityBase
        {
            public bool Equals(T x, T y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(T obj)
            {
                int hCode = obj.Id.GetHashCode();
                return hCode;
            }
        }

        public class EntityBaseEqualityComparer : IEqualityComparer<EntityBase>
        {
            public bool Equals(EntityBase x, EntityBase y)
            {
                return x.GetType() == y.GetType() && x.Id == y.Id;
            }

            public int GetHashCode(EntityBase obj)
            {
                int hCode = obj.GetType().GUID.GetHashCode();
                return hCode.GetHashCode();
            }
        }
    }
}
