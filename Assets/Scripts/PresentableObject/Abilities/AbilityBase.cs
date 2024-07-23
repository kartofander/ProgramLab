using System;
using System.Collections;

namespace ProgramLab
{
    [Serializable]
    public abstract class AbilityBase
    {
        public abstract void Init();
        public abstract IEnumerator Perform();

        public virtual void Reset()
        {
            
        }
        public virtual void OnStop()
        {
        }
    }
}