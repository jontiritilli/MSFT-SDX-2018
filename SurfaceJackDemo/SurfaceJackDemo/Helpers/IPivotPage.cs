using System.Threading.Tasks;

namespace SurfaceJackDemo.Helpers
{
    public interface IPivotPage
    {
        Task OnPivotSelectedAsync();

        Task OnPivotUnselectedAsync();
    }
}
