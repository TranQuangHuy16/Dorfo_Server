using Dorfo.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Shared.Helpers
{
    public class OptionsEqualHelper
    {
        public bool OptionsEqual(List<CartItemOptionResponse> opts1, List<CartItemOptionResponse> opts2)
        {
            if (opts1.Count != opts2.Count) return false;

            foreach (var o1 in opts1)
            {
                var o2 = opts2.FirstOrDefault(x => x.OptionId == o1.OptionId);
                if (o2 == null) return false;

                var values1 = o1.SelectedValues.Select(v => v.OptionValueId).OrderBy(x => x);
                var values2 = o2.SelectedValues.Select(v => v.OptionValueId).OrderBy(x => x);

                if (!values1.SequenceEqual(values2)) return false;
            }

            return true;
        }

    }
}
