using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListNodeSimple
{
    /// <summary>
    /// Интерфейс сереализации
    /// </summary>
    public interface ISerializable
    {
        /// <summary>
        /// Интерфейс десереализации
        /// </summary>
        /// <param name="s"></param>
        public void Serialize(Stream s);
    }
}
