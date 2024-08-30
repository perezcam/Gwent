using System;
using System.Collections.Generic;

namespace GameLogic
{ 
    public class LogicGameManager
    {
        //Humanity Deck
        public static Card Armin = new Card("Armin Arlet","Humanity",  "Carta de Héroe: Estratega capaz de cambiar el rumbo de la batalla con su agudo intelecto, unificando las fuerzas de todos los guerreros para vencer",8, 1, null, Functions.AvaragedPower, new List<int>{2} , 0);
        public static Card AtaqueNocturno = new Card("Ataque Nocturno","Humanity", "Carta Clima: Ataque sorpresa nocturno que debilita la primera linea enemiga, reduciendo el poder de las cartas con capacidad de ataque 4 o inferior a 0.",0, 2, null, Functions.W_ReducePowerOfWeakCards, new List<int>{1}, 1);
        public static Card EstrategiaErwin = new Card("Estrategia Erwin","Humanity","Carta Incremento: Moviliza todas las unidades para una carga decisiva, impulsando el espíritu combativo del ejercito aumentando en su poder el poder de cada carta en la fila", 3, 3, null, Functions.IncreasePowerRow, new List<int>{3}, 1);
        public static Card Connie = new Card("Connie Springer","Humanity", "Carta de Unidad: Rápido y ágil, excepcional en ataques veloces.",4, 4, null, Functions.NullFunction, new List<int>{1}, 0);
        public static Card EM3D = new Card("EM3D","Humanity","Carta Incremento: Permite un movimiento y evasión rápidos, esencial para el combate moderno por lo que aumenta la capacidad de ataque de cada carta en su poder." ,2, 5, null, Functions.IncreasePowerRow, new List<int>{3}, 1);
        public static Card Eren = new Card("Eren Yeager","Humanity","Carta de Héroe: Posee el poder de transformarse y aplastar enemigos con fuerza devastadora.Elimina al enemigo mas poderoso" ,11, 6, null, Functions.DelPowerfullCard, new List<int>{1}, 2);
        public static Card Erwin = new Card("Erwin Smith","Humanity","Carta de Unidad: Su liderazgo aumenta la moral y el poder de todos los aliados en batalla, sacrificara su brazo para eliminar al mas debil del enemigo", 6, 7, null, Functions.DelWeakestCard, new List<int>{1}, 0);
        public static Card EstrategiaLevi = new Card("Estrategia Levi","Humanity","Carta Unidad: Una maniobra táctica que corta las defensas enemigas con precisión quirúrgica.Eliminara facilmente la fila con menos cartas", 4, 8, null, Functions.CleanRow, new List<int>{1}, 0);
        public static Card Flegel = new Card("Flegel Reeves","Humanity" ,"Carta de Unidad: Inspira el apoyo local, proporcionando refuerzos y esperanza. Tiene la capacidad de lograr un despeje efectivo",3, 9, null, Functions.RemoveWeatherCard, new List<int>{3}, 0);
        public static Card Gabi = new Card("Gabi Braun","Humanity","Carta de Unidad: Joven e imprudente, capaz de daños inesperados y dramáticos. Es un buen recurso que aumentara el poder de los que luchan a su lado", 5, 10, null, Functions.IncreasePowerRow, new List<int>{2}, 0);
        public static Card Hange = new Card("Hange Zoë","Humanity", "Lider de la faccion de los humanos. Con sus conocimientos cientificos basados en prueba y error es capaz de concederte la victoria en caso de empate",5, 11, null, Functions.NullFunction, new List<int>{10}, 3);
        public static Card Hannes = new Card("Hannes","Humanity", "Carta de Unidad: Soldado veterano, proporciona una defensa sólida contra los ataques.Puede inhabilitar un clima",4, 12, null, Functions.RemoveWeatherCard, new List<int>{1}, 0);
        public static Card Historia = new Card("Historia Reiss","Humanity","Carta de Unidad: Empodera y protege a los aliados cercanos con su presencia real.No tiene habilidad especial definida" ,5, 13, null, Functions.StealCard, new List<int>{3}, 0);
        public static Card Jean = new Card("Jean Kirstein","Humanity", "Carta de Unidad: Luchador adaptable, eficaz en múltiples escenarios de combate.",6, 14, null, Functions.AttackMultiplier, new List<int>{1}, 0);
        public static Card Levi = new Card("Levi Ackerman","Humanity","Carta de Unidad: Extremadamente letal en el ataque, puede diezmar líneas enemigas enteras.Y en ocasiones si sus tropas lo traicionan tambien las eliminara", 9, 15, null, Functions.CleanRow, new List<int>{1}, 0);
        public static Card Magaly = new Card("Magaly","Humanity","Carta de Unidad: Guerrera menos conocida, sorprendentemente eficaz en el combate Quita incrementos y le da el poder original a las cartas.", 4, 16, null, Functions.W_ResetCardValues, new List<int>{2}, 0);
        public static Card ManiobraDistraccion = new Card("Maniobra de Distracción","Humanity","Carta Clima: Desvía la atención del enemigo,sus incrementos se veran perjudicados, bajando su guardia, pero provoca perdidas de puntos tambien a las cartas que permanecen en la fila propia.", 0, 17, null, Functions.W_ResetCardValues, new List<int>{2}, 1);
        public static Card Mikasa = new Card("Mikasa Ackerman","Humanity", "Carta de Héroe: Combatiente altamente capacitada, inflige daños significativos a sus enemigos.Puede utilizar todas las cartaas del campo para aumentar el poder de cada una en su promedio",8, 18, null, Functions.AvaragedPower, new List<int>{1}, 2);
        public static Card Nanaba = new Card("Nanaba","Humanity","Carta de Unidad: Experta en movilidad y enfrentamiento con el enemigo.Puede eliminar al mas debil enemiogo", 4, 19, null, Functions.DelWeakestCard, new List<int>{2}, 0);
        public static Card Petra = new Card("Petra Ral","Humanity", "Carta de Unidad: Apoya a sus aliados con habilidades que mejoran sus capacidades.Te permite obtener mas apoyo al incrementa 1 carta a tu mano",4, 20, null, Functions.StealCard, new List<int>{2}, 0);
        public static Card RefuerzosTrost = new Card("Refuerzos de Trost","Humanity","Carta Incremento: Fortalece la línea más débil, proporcionando apoyo crucial, dandole mas poder a las cartas  de su fila para defender la ciudad.", 1, 21, null, Functions.IncreasePowerRow, new List<int>{2}, 1);
        public static Card Sasha = new Card("Sasha Blouse","Humanity", "Carta de Unidad: Francotiradora, puede apuntar a unidades clave enemigas desde la distancia.Podra eliminar a el mas poderoso enemigo desde lejos",6, 22, null, Functions.DelPowerfullCard, new List<int>{2}, 0);
        public static Card Takami = new Card("Takami","Humanity","Carta de Unidad: Un personaje menor con un papel en el reconocimiento y la inteligencia. reduce el poder de las cartas debiles a cero", 3, 23, null, Functions.W_ReducePowerOfWeakCards, new List<int>{3}, 0);
        public static Card Ymir = new Card("Ymir","Humanity","Carta de Unidad: Lucha ferozmente por sus amigos, impredecible en la batalla.Es capaz de darlo todo para lograr que la fuerza de sus companeros aumente", 5, 24, null, Functions.IncreasePowerRow, new List<int>{1}, 0);
        public static Card Marco = new Card("Marco Bott","Humanity","Carta de Unidad: Puede sustituir a cualquier carta del campo y si sustituye a una carta de unidad con capacidad de obtener un clima te permitira tener un duplicado de la misma", 0, 25, null, Functions.NullFunction, new List<int>{3}, 4);

        //Titans Deck
        public static Card TitanBestia = new Card("T.Bestia","Titans","Lider de la faccion de los titanes con su rugido puede invocar una carta extra al inicio de cada ronda", 0, 26, null, Functions.NullFunction, new List<int>{10}, 3);
        public static Card TitanFundador = new Card("T.Fundador","Titans","Carta de Unidad: Capaz de alterar la memoria de los humanos y controlar otros titanes, incrementando el poder de ataque de cada carta de su fila " ,2, 27, null, Functions.IncreasePowerRow, new List<int>{1}, 0);
        public static Card TitanAcorazado = new Card("T.Acorazado","Titans","Carta de Unidad: Con una defensa impenetrable y fuerza abrumadora es capaz de destruir al mas debil de los enemigos.",5, 28, null, Functions.DelWeakestCard, new List<int>{1}, 0);
        public static Card TitanHembra = new Card("T.Hembra","Titans","Carta Heroe: Rápida y letal, con habilidades tácticas superiores, es capaz de destruir al mas fuerte de los enemigos.",6, 29, null, Functions.DelPowerfullCard, new List<int>{2}, 2);
        public static Card TitanColosal = new Card("T.Colosal","Titans","Carta de Héroe: Su tamaño y poder pueden destruir filas enteras al instante, destruira la fila propia o enemiga con menor cantidad de cartas.",10, 30, null, Functions.CleanRow, new List<int>{3}, 2);
        public static Card TitanSantaClaus = new Card("T.Santa Claus","Titans","Carta de Unidad: Un titán festivo pero peligroso, distribuye caos en lugar de regalos.No permitira cartas de incremento en sus filas", 0, 31, null, Functions.W_ResetCardValues, new List<int>{2}, 1);
        public static Card TitanDentada = new Card("T.Dentada","Titans","Carta de Unidad: Con dientes afilados, ideal para combates cercanos y desgarradores. No tiene habilidad especial definida", 5, 32, null, Functions.NullFunction, new List<int>{1}, 0);
        public static Card TitanGloton = new Card("T.Glotón","Titans","Carta de Unidad: Devora todo a su paso, si su enemigo es el mas debil lo destruira.", 4, 33, null, Functions.DelPowerfullCard, new List<int>{1}, 0);
        public static Card TitanObervador = new Card("T.Observador","Titans","Carta Unidad: Utiliza su altura para proporcionar ventajas estratégicas y visuales. No tiene habilidad especial conocida", 3, 34, null, Functions.NullFunction, new List<int>{2}, 0);
        public static Card TitanSonriente = new Card("T.Sonriente","Titans","Carta de Unidad: Su sonrisa enmascara una ferocidad implacable. No tiene habilidad especial definida", 5, 35, null, Functions.NullFunction, new List<int>{1}, 0);
        public static Card TitanSaltarin = new Card("T.Saltarín","Titans","Carta de Unidad: Ágil y difícil de atrapar, salta por el campo de batalla causando estragos. Tiene la habilidad de ponerse en el lugar de otra cartas, si la cambias por una carta de unidad con funcion de clima obtendras un duplicado", 0, 36, null, Functions.Decoy, new List<int>{2}, 4);
        public static Card TitanAgraciado = new Card("T.Agraciado","Titans","Carta de Unidad: Engaña con su apariencia calmada, pero es mortal en el ataque. No tiene habilidad especial definida", 4, 37, null, Functions.NullFunction, new List<int>{2}, 0);
        public static Card TitanCarguero = new Card("T.Carguero","Titans","Carta de Heroe: Transporta otros titanes más pequeños al campo de batalla.Estableciendo el poder carta de campo como el promedio de todas.", 6, 38, null, Functions.AvaragedPower, new List<int>{3}, 0);
        public static Card TitanMandibula = new Card("T.Mandíbula","Titans","Carta de Heroe: Rápido y con mandíbulas que pueden triturar casi cualquier cosa.Destruira al mas fuerte enemigo", 6, 39, null, Functions.DelPowerfullCard, new List<int>{1}, 0);
        public static Card TitanMartillodeGuerra = new Card("T.WarHammer","Titans","Carta de Despeje: Forja armas de energía titánica en el combate, que le permiten quitar cualquier carta clima presente en su fila", 7, 40, null, Functions.RemoveWeatherCard, new List<int>{3}, 0);
        public static Card TitanFalco = new Card("T.Falco","Titans","Carta de Despeje: Un nuevo Titán con la capacidad de inhabilitar un clima con su vuelo.", 4, 41, null, Functions.RemoveWeatherCard, new List<int>{1}, 0);
        public static Card TitanDesCarado = new Card("T.Descarado","Titans","Carta de Unidad: Sin reservas en su comportamiento, ataca con desenfreno.", 4, 42, null, Functions.NullFunction, new List<int>{1}, 0);
        public static Card TitanParlante = new Card("T.Parlante","Titans","Carta Especial: Negocia o distrae a sus enemigos con palabras antes de atacar, aumeta su poder en la cantidad de cartas iguales a ella en el campo.", 3, 43, null, Functions.AttackMultiplier, new List<int>{2}, 0);
        public static Card TitanTatin = new Card("T.Tatin","Titans","Carta de Unidad: Pequeño pero sorprendentemente fuerte, un titán de bolsillo, Su habilidad especial es la de incrementar la fuera de los que lo rodean.", 2, 44, null, Functions.IncreasePowerRow, new List<int>{1}, 0);
        public static Card CalorSofocante = new Card("Calor Sofocante","Titans","Carta de Clima: Aumenta la temperatura del campo de batalla, debilitando a las unidades con poder menor o igual que 4 que estan activas en su fila.", 0, 45, null, Functions.W_ReducePowerOfWeakCards,new List<int>{1}, 1);
        public static Card Caminos = new Card("Caminos","Titans","Carta de Incremento: Conecta los destinos de todos los titanes, permitiendo maniobras coordinadas.Incrementara el poder de sus aliados", 2, 46, null, Functions.IncreasePowerRow, new List<int>{3}, 1);
        public static Card ElDespertar = new Card("El Despertar","Titans","Carta Incremento: Transforma temporalmente a los humanos en titanes, aumentando el poder de cada carta en su poder.", 0, 47, null, Functions.W_ResetCardValues, new List<int>{3}, 1);
        public static Card SuerodeTitan = new Card("Suero de Titán","Titans","Carta Incremento: Transforma temporalmente a los humanos en titanes, aumentando el poder de cada carta en su poder.", 2, 48, null, Functions.IncreasePowerRow, new List<int>{2}, 1);
        public static Card FuriaTitan = new Card("Furia de Titán","Titans","Carta Incremento: Incrementa la ferocidad de los titanes en el campo, mejorando su ataque.", 3, 49, null, Functions.IncreasePowerRow, new List<int>{1}, 1);
        public static Card LLuviadeRocas = new Card("Lluvia de Rocas","Titans","Carta de Clima: Lanza rocas gigantes desde el cielo, dañando a todas las unidades , propias y enemigas de su fil. Evita el uso de cartas de incremeto.", 0, 50, null, Functions.W_ResetCardValues, new List<int>{3}, 1);
        public static Card SuenodeFundador = new Card("Sueño de Fundador","Titans","Carta Clima: Un poderoso efecto que altera el estado del juego, reconfigurando el campo de batalla, al disminuir el poder de cada carta debil colocada en ese tipo de fila que le corresponde a cero.", 0, 51, null, Functions.W_ReducePowerOfWeakCards, new List<int>{2}, 1);
        public static Card RugidodelTitanBestia = new Card("Rugido T.Bestia","Titans","Carta de Despeje: El rugido ensordecedor disminuye la moral enemiga, reduciendo su eficacia.", 2, 52, null, Functions.RemoveWeatherCard, new List<int>{2}, 0);

        public static List<Card> CardDataBase = new List<Card>
        {
            Armin, AtaqueNocturno, EstrategiaErwin, Connie, EM3D, Eren, Erwin, EstrategiaLevi, Flegel, Gabi, Hange, Hannes, Historia, Jean, Levi, Magaly, ManiobraDistraccion, Mikasa, Nanaba, Petra, RefuerzosTrost, Sasha, Takami, Ymir, Marco,
            TitanBestia, TitanFundador, TitanAcorazado, TitanHembra, TitanColosal, TitanSantaClaus, TitanDentada, TitanGloton, TitanObervador, TitanSonriente, TitanSaltarin, TitanAgraciado, TitanCarguero, TitanMandibula, TitanMartillodeGuerra, TitanFalco, TitanDesCarado, TitanParlante, TitanTatin, CalorSofocante, Caminos, ElDespertar, SuerodeTitan, FuriaTitan, LLuviadeRocas, SuenodeFundador, RugidodelTitanBestia
        };
        public static LogicGameManager instance;
        public int currenTurn = 0;
        public int currentID = 53;
        public static Dictionary<int, Card> CardDictionary = new Dictionary<int, Card>();
        public Player player1 { get; set; }
        public Player player2 { get; set; }
        public LogicGameManager(string nick1, string faction1, string nick2, string faction2)
        {
            InitializeCardDictionary();
            player1 = new Player(nick1, faction1, CardDictionary);
            player2 = new Player(nick2, faction2, CardDictionary);
            player1.enemy = player2;
            player2.enemy = player1;
            player1.SetContext();
            player2.SetContext();
            instance = this;
        }
        private static void InitializeCardDictionary()
        {
            foreach (Card card in CardDataBase)
            {
                if (!CardDictionary.ContainsKey(card.ID))
                {
                    CardDictionary.Add(card.ID, card);
                }
            }   
        }
        
 
        public  Player PlayerOnTurn()
        {
            if(currenTurn%2==0)
            {
                player1.OnTurn = true;
                player2.OnTurn = false;
                return player1;
            }
            player2.OnTurn=true;
            player1.OnTurn=false;
            return player2;
        }
    }
}

