//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class PlayersByGame
    {
        public int gameId { get; set; }
        public int playerId { get; set; }
        public string tipo { get; set; }
        public int playersByGameId { get; set; }
    
        public virtual Game Game { get; set; }
        public virtual Player Player { get; set; }
    }
}
