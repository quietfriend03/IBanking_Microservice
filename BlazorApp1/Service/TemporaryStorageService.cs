using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;

namespace BlazorApp1.Service
{
    public class TemporaryStorageService
    {
        private readonly IJSRuntime _runtime;
        private string _mssvKey = "mssv";
        private string _hoadonKey = "hoaDonId";

        public TemporaryStorageService(IJSRuntime runtime)
        {
            _runtime = runtime;
        }

        public async Task SetMSSV(int MSSV)
        {
            await _runtime.InvokeVoidAsync("sessionStorage.setItem", _mssvKey, MSSV);
        }

        public async Task SetHoaDonId(int id)
        {
            await _runtime.InvokeVoidAsync("sessionStorage.setItem", _hoadonKey, id);
        }

        public async Task<int?> GetMSSV()
        {
            string mssvString = await _runtime.InvokeAsync<string>("sessionStorage.getItem", _mssvKey);
            if (int.TryParse(mssvString, out int mssv))
            {
                return mssv;
            }
            return null;
        }

        public async Task<int?> GetHoaDonId()
        {
            string hoaDonIdString = await _runtime.InvokeAsync<string>("sessionStorage.getItem", _hoadonKey);
            if (int.TryParse(hoaDonIdString, out int hoaDonId))
            {
                return hoaDonId;
            }
            return null;
        }

        public async Task PaymentComplete()
        {
            await _runtime.InvokeVoidAsync("sessionStorage.removeItem", _mssvKey);
            await _runtime.InvokeVoidAsync("sessionStorage.removeItem", _hoadonKey);
        }
    }
}
