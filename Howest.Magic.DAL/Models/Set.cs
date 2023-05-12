﻿#nullable disable

namespace Howest.MagicCards.DAL.Models
{
	public partial class Set
    {
        public Set()
        {
            Cards = new HashSet<Card>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }
}
