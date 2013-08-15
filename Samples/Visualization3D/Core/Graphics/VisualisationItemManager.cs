using CSCore.Utils;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D9;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Visualization3D.Core.Graphics
{
    public class VisualisationItemManager<T> : SceneNode where T : IVisualisationItem
    {
        private int _itemCount;
        private Device _device;
        private Context _context;

        public VisualisationItemManager(Context context, int itemCount)
        {
            _itemCount = itemCount;
            _device = context.Device;
            _context = context;
        }

        public override void Load()
        {
            for (int i = 0; i < _itemCount; i++)
            {
                var item = Activator.CreateInstance<T>();
                item.Context = _context;
                Childs.Add(item);
            }

            base.Load();
        }

        public override void Update(float time)
        {
            base.Update(time);
        }

        public override void Render(Matrix worldViewProj, float time)
        {
            ((T)Childs.First()).BeginItemRendering();

            float offset = (-Childs.Count * 3f) / 2f;
            foreach (var item in Childs)
            {
                Matrix matrix = Matrix.Translation(new Vector3(offset, 0, 0));
                ((T)item).Render(matrix * worldViewProj, time);
                offset += 3f;
            }

            ((T)Childs.First()).EndItemRendering();
        }

        public void SetValue(int index, float value)
        {
            ((T)Childs[index]).Value = value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in Childs)
                {
                    item.Dispose();
                }
                Childs.Clear();
            }
        }

        ~VisualisationItemManager()
        {
            Dispose(false);
        }
    }

    public class ResourceManager
    {
        private static ResourceManager _instance;

        public ResourceManager Instance
        {
            get { return _instance ?? (_instance = new ResourceManager()); }
        }

        private Dictionary<string, IDisposable> _resources;

        public ResourceManager()
        {
            _resources = new Dictionary<string, IDisposable>();
        }

        public void AddResource(string key, IDisposable resource)
        {
            _resources.Add(key, resource);
        }

        public T GetResource<T>(string key) where T : IDisposable
        {
            return (T)GetResource(key);
        }

        public IDisposable GetResource(string key)
        {
            IDisposable value;
            if (_resources.TryGetValue(key, out value))
                return value;
            return null;
        }

        public void FreeResource(string key, bool remove = true)
        {
            var resource = GetResource(key);
            resource.Dispose();
            if (remove)
                _resources.Remove(key);
        }

        public void FreeAll()
        {
            foreach (var item in _resources)
            {
                FreeResource(item.Key, false);
            }
            _resources.Clear();
        }
    }
}