using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListNodeSimple
{
    /// <summary>
    /// Интерфейс сереализации и десереализации списка
    /// </summary>
    public interface IListSerialize : ISerializable, IDeserializable
    {
        /// <summary>
        /// Вставить элемент
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void InsertElement(int index, string value);

        /// <summary>
        /// Получить список элементов
        /// </summary>
        /// <returns></returns>
        public List<string> GetElements();
    }
}
