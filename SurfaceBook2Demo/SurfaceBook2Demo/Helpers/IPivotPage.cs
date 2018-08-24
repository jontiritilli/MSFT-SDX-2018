using System.Threading.Tasks;

namespace SurfaceBook2Demo.Helpers
{
    public interface IPivotPage
    {
        Task OnPivotSelectedAsync();

        Task OnPivotUnselectedAsync();
    }
}
