using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection
{
    public interface IBookFinderService
    {
        Book FindBook(string author, string name);
    }
}
