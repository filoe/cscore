using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visualization3D.Core
{
    public class SceneNode : IRenderComponent
    {
        private List<IComponent> _childs;

        public List<IComponent> Childs
        {
            get { return _childs ?? (_childs = new List<IComponent>()); }
            set { _childs = value; }
        }

        public virtual void Render(Matrix worldViewProj, float time)
        {
            foreach (var item in Childs.Where((x) => x is IRenderComponent))
            {
                ((IRenderComponent)item).Render(worldViewProj, time);
            }
        }

        public virtual void Load()
        {
            foreach (var item in Childs)
            {
                item.Load();
            }
        }

        public virtual void Update(float time)
        {
            foreach (var item in Childs)
            {
                item.Update(time);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //dispose managed
            }
            foreach (var item in Childs)
            {
                item.Dispose();
            }
        }

        ~SceneNode()
        {
            Dispose(false);
        }
    }
}