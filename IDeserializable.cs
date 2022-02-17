using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListNodeSimple
{
    /// <summary>
    /// Интерфейс десериализациии
    /// </summary>
    public interface IDeserializable
    {
        /// <summary>
        /// Десереализовать
        /// </summary>
        /// <param name="s"></param>
        public void Deserialize(Stream s);
    }
}
