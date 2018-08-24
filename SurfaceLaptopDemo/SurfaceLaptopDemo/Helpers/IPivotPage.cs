using System.Threading.Tasks;

namespace SurfaceLaptopDemo.Helpers
{
    public interface IPivotPage
    {
        Task OnPivotSelectedAsync();

        Task OnPivotUnselectedAsync();
    }
}
