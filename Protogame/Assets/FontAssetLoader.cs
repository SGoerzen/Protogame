//
// This source code is licensed in accordance with the licensing outlined
// on the main Tychaia website (www.tychaia.com).  Changes to the
// license on the website apply retroactively.
//
using System;

namespace Protogame
{
    public class FontAssetLoader : IAssetLoader
    {
        private IAssetContentManager m_AssetContentManager;
        
        public FontAssetLoader(IAssetContentManager assetContentManager)
        {
            this.m_AssetContentManager = assetContentManager;
        }
    
        public bool CanHandle(dynamic data)
        {
            return data.Loader == typeof(FontAssetLoader).FullName;
        }

        public IAsset Handle(string name, dynamic data)
        {
            return new FontAsset(
                this.m_AssetContentManager,
                name,
                data.FontName,
                data.FontSize,
                data.FontData);
        }

        public IAsset GetDefault(string name)
        {
            throw new InvalidOperationException();
        }
    }
}

