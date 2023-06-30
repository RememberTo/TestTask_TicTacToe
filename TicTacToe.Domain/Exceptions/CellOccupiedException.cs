using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Common.Exceptions
{
    public class CellOccupiedException : Exception
    {
        public CellOccupiedException(int X, int Y)
        : base($"Cell {{{X}:{Y}}} occupied") {
        }
    }
}
