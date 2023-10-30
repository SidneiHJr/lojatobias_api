
namespace LojaTobias.Api.Core.Models
{
    public abstract class FiltroModelBase
    {
        public string DirecaoOrdem { get; set; } = "asc";
        public string? ColunaOrdem { get; set; }
    }
}
