using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumberjack
{
    public class Collidable
    {
        #region Fields
        protected BoundingSphere boundingSphere = new BoundingSphere();
        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
        }
        #endregion

        #region Member Functions
        public virtual bool CollisionTest(Collidable obj)
        {
            if (obj != null)
            {
                return BoundingSphere.Intersects(obj.BoundingSphere);
            }

            return false;
        }

        public virtual void OnCollision(Collidable obj)
        {
           
        }
        #endregion


    }
}
