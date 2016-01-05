using System;
using System.Diagnostics;

namespace SurfnetSSO {
    public sealed class WebLauncher : IWebLauncher {
        public void Launch(Uri uri) {
            Process.Start(uri.OriginalString);
        }
    }
}
