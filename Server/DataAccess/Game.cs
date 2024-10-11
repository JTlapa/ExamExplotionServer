namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Game")]
    public partial class Game
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Game()
        {
            Player = new HashSet<Player>();
        }

        public int gameId { get; set; }

        [StringLength(50)]
        public string invitationCode { get; set; }

        [StringLength(20)]
        public string gameStatus { get; set; }

        public int? numberPlayers { get; set; }

        public int? timePerTurn { get; set; }

        public int? lives { get; set; }

        public int? winnerPlayerId { get; set; }

        public int hostPlayerId { get; set; }

        public virtual Users Users { get; set; }

        public virtual Users Users1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Player> Player { get; set; }
    }
}
