using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace crossword.engine
{
    public class CrosswordDatabaseItem
    {
        public string question;
        public string answer;

        public CrosswordDatabaseItem(string question, string answer)
        {
            this.question = question;
            this.answer = answer;
        }

        public bool AnswerStartsWith(char c)
        {
            if (string.IsNullOrEmpty(answer)) return false;
            return answer[0] == c;
        }

        public bool AnswerCharCount(int n)
        {
            if (string.IsNullOrEmpty(answer)) return false;
            return answer.Length == n;
        }

        public override string ToString()
        {
            return string.Format("[CrosswordDatabaseItem] QUESTION: {0} ~ ANSWER: {1}", question, answer);
        }
    }

    public class CrosswordsDatabase
    {

        List<CrosswordDatabaseItem> database = new List<CrosswordDatabaseItem>();

        public void SetupDatabase()
        {
            AddItemToDatabase("Vênus, no Sistema Solar (Astr.)", "SEGUNDOPLANETA");
            AddItemToDatabase("Telúrio (símbolo)", "TE");
            AddItemToDatabase("Brado em touradas", "OLE");
            AddItemToDatabase("Principal cômodo da casa", "SALA");
            AddItemToDatabase("Capacidade", "POTENCIALIDADE");
            AddItemToDatabase("Famosa fábula de Esopo (Lit.)", "ARAPOSAEASUVAS");
            AddItemToDatabase("O maior mamífero da América do Sul", "ANTA");
            AddItemToDatabase("Janela horizontal fixa ou móvel de carros de luxo", "TETOSOLAR");
            AddItemToDatabase("Sobremesa gelada e colorida", "GELATINA");
            AddItemToDatabase("Aquele indivíduo", "ELE");
            AddItemToDatabase("Torneira, em inglês", "TAP");
            AddItemToDatabase("N", "ENE");
            AddItemToDatabase("A (?): ao acaso", "ESMO");
            AddItemToDatabase("Apêndices de aves", "ASAS");
            AddItemToDatabase("A sílaba fraca da palavra (Gram.)", "ATONA");
            AddItemToDatabase("(?) Rosa, atriz", "ANA");
            AddItemToDatabase("Excursão do artista", "TURNE");
            AddItemToDatabase("(?) de saúde: hospitais", "CASAS");
            AddItemToDatabase("Doença como a sífilis (sigla)", "DST");
            AddItemToDatabase("Significado do \"C\", em \"PCB\"", "");
            AddItemToDatabase("\"Orelha\" em \"aurícula\"", "AURI");
            AddItemToDatabase("(?)-aviões, navio que serve como base aérea móvel", "PORTA");
            AddItemToDatabase("(?) Tostoi, romancista russo", "LIV");
            AddItemToDatabase("(?) Piovani, atriz brasileira", "LUANA");
            AddItemToDatabase("Ervas (?), platas com ação anti-inflamatória", "BALEEIRAS");
            AddItemToDatabase("Dá ordens ao Gênio (Lit.)", "AMO");
            AddItemToDatabase("Contratar, em inglês", "HIRE");
            AddItemToDatabase("Agente transmissor da Aids", "HIV");
            AddItemToDatabase("401, em romanos", "CDI");
            AddItemToDatabase("Ana Néri", "AN");
            AddItemToDatabase("A mãe de Abel (Bíblia)", "EVA");
            AddItemToDatabase("Imigrantes orientais da Liberdade (SP)", "JAPONESES");
            AddItemToDatabase("A fruta própria para o consumo", "MADURA");
            AddItemToDatabase("Trabalhador informal de estacionamentos", "GUARDADOR");
            AddItemToDatabase("Loja com várias seções como a de frios", "SUPERMERCADO");
            AddItemToDatabase("Artefato bélico", "ARMA");
            AddItemToDatabase("Remexer; remover", "REVIRAR");
            AddItemToDatabase("Gerido; governado", "ADMINISTRADO");
            AddItemToDatabase("Suspiros poéticos", "AIS");
            AddItemToDatabase("O Ser como Deus", "TODOPODEROSO");
            AddItemToDatabase("Item do cheque", "DATA");
            AddItemToDatabase("Sua capital é Porto Velho (sigla)", "RO");
            AddItemToDatabase("Que foi sorteado", "PREMIADO");
            AddItemToDatabase("Estimado", "BEMVISTO");
            AddItemToDatabase("\"(?) Man\", filme com Dustin Hoffman", "RAIN");
            AddItemToDatabase("(?) Parker, cineasta de \"The Wall\"", "ALAN");
            AddItemToDatabase("Tipo de peneira", "APA");
            AddItemToDatabase("Cinto; cintura", "COS");
            AddItemToDatabase("Transferida (a verba)", "REPASSADA");
            AddItemToDatabase("O limão, pela quantidade de vitamina C", "RICO");
            AddItemToDatabase("Ação entra (?): rifa", "AMIGOS");
            AddItemToDatabase("(?) cream: sorvete (ing.)", "ICE");
            AddItemToDatabase("Morto, em inglês", "DEAD");
            AddItemToDatabase("Desmonorar; desabar", "RUIR");
            AddItemToDatabase("Nome da letra que indica sono na HQ (pl.)", "ZES");
            AddItemToDatabase("Medonhos; feíssimos", "HORROROSOS");
            AddItemToDatabase("Lutécio (símbolo)", "LU");
            AddItemToDatabase("Indivíduo cruel; carrasco (fig.)", "ALGOZ");
            AddItemToDatabase("Sulfixo de \"cirrose\"", "OSE");
            AddItemToDatabase("Fluido usado no lança-perfume", "ETER");
            AddItemToDatabase("(?) at Work, banda de rock australiana", "MEN");
            AddItemToDatabase("Assalariada que trabalha em lares", "DOMESTICA");
            AddItemToDatabase("Serviço coberto por planos de saúde", "CONSULTAMEDICA");
            AddItemToDatabase("Acompanhado pela polícia", "ESCOLTADO");
            AddItemToDatabase("Local de aluguel de vagas para carros", "ESTACIONAMENTO");
            AddItemToDatabase("Indicativo (abrev.)", "IND");
            AddItemToDatabase("Utensílio para limpar pisos de madeira", "VASSOURADEPELO");
            AddItemToDatabase("Fazer a última refeição", "CEAR");
            AddItemToDatabase("Bolsa de supermercado", "SACA");
            AddItemToDatabase("The Rolling (?), banda de rock", "STONES");
            AddItemToDatabase("Dificuldade; aperto (pop.)", "SUFOCO");
            AddItemToDatabase("De + as", "DAS");
            AddItemToDatabase("Sílaba de \"orca\"", "OR");
            AddItemToDatabase("O sentido da pele", "TATO");
            AddItemToDatabase("Instrumento de anjos", "LIRA");
            AddItemToDatabase("(?) Babá, herói de conto infantil", "ALI");
            AddItemToDatabase("Ave negra insetívora", "ANU");
            AddItemToDatabase("Planta cuja folha é usada para assar peixe", "BANANEIRA");
            AddItemToDatabase("Casa (fig.)", "LAR");
            AddItemToDatabase("O Cupido (Mit.)", "EROS");
            AddItemToDatabase("Alterar; modificar", "MUDAR");
            AddItemToDatabase("Regina Duarte, atriz brasileira", "RD");
            AddItemToDatabase("Interjeição de cólera típica do nordeste", "ARRE");
            AddItemToDatabase("Sigla do Distrito Federal", "DF");
            AddItemToDatabase("\"Ombro\" em \"omoplata\"", "OMO");
            AddItemToDatabase("Extensão da terra semeada", "SEARA");
            AddItemToDatabase("Osso da coxa", "FEMUR");
            AddItemToDatabase("País árabe com menor IDH", "IEMEN");
            AddItemToDatabase("Virada (?), evento artístico com duração de 24 horas", "CULTURAL");
            AddItemToDatabase("Coautor de \"Ai que Saudades da Amélia\"", "MARIOLAGO");
            AddItemToDatabase("(?) logo: é dito na despedida", "ATE");
            AddItemToDatabase("Profeta hebreu", "ELI");
            AddItemToDatabase("Nome da 19ª letra", "ESSE");
            AddItemToDatabase("Evento do Sete de Setembro no Brasil", "DESFILEMILITAR");
            AddItemToDatabase("Remédio de eficácia momentânea", "PALIATIVO");
            AddItemToDatabase("Santiago (Geog.)", "CAPITALCHILENA");
            AddItemToDatabase("Laboratório (Abrev.)", "LAB");
            AddItemToDatabase("O livro, em relação ao ensino", "MATERIALDIDATICO");
            AddItemToDatabase("Classificação do fetiche sexual", "TARA");
            AddItemToDatabase("Congênita", "INATA");
            AddItemToDatabase("Causador de problemas cardíacos, comun em pessoas obesas", "COLESTEROLALTO");
            AddItemToDatabase("Maior Planície inundável do mundo (BR)", "PANTANAL");
            AddItemToDatabase("Período; época", "ERA");
            AddItemToDatabase("Vantagem sugerida na depilação a laser", "INDOLOR");
            AddItemToDatabase("Antigo (abrev.)", "ANT");
            AddItemToDatabase("A ligação \"0800\"", "GRATIS");
            AddItemToDatabase("O amor platônico", "IDEAL");
            AddItemToDatabase("País onde morreu a ativista social Zilda Arns, vítima de um terremoto", "HAITI");
            AddItemToDatabase("Poeta grego", "AEDO");
            AddItemToDatabase("Brilho", "ALVOR");
            AddItemToDatabase("Mandar (um e-mail)", "ENVIAR");
            AddItemToDatabase("O polo dos esquimós (abrev.)", "N");
            AddItemToDatabase("Instituíção onde se abrigam menores", "PATRONATO");
            AddItemToDatabase("Documento do veículo", "DUT");
            AddItemToDatabase("Aperto de mão no jogo salada mista (inf.)", "PERA");
            AddItemToDatabase("\"Lésbicas\", em LGBT", "L");
            AddItemToDatabase("Extravio; sumiço", "PERDA");
            AddItemToDatabase("A primeira nota musical", "DO");
            AddItemToDatabase("Estampa o pôster das fãs", "IDOLO");
            AddItemToDatabase("Condição de Chico Buarque na ditadura", "EXILADO");
            AddItemToDatabase("Oswaldo Cruz, médico sanitarista", "OC");
            AddItemToDatabase("Aparelho para acionar o fogão", "ACENDEDOR");
            AddItemToDatabase("Cada facção de um partido político", "ALA");
            AddItemToDatabase("Duas vezes", "BI");
            AddItemToDatabase("Desatento", "DESCONCENTRADO");
            AddItemToDatabase("O \"braço\" do polvo", "TENTACULO");
            AddItemToDatabase("(?) Pelada, região que ficou conhecida pela sua abundância em ouro (PA)", "SERRA");
            AddItemToDatabase("Mudar de crença religiosa", "CONVERTER");
            AddItemToDatabase("Hora canônica no Catolicismo", "NOA");
            AddItemToDatabase("Gato, em inglês", "CAT");
            AddItemToDatabase("Imoral; obsceno", "INDECENTE");
            AddItemToDatabase("\"Transtorno\", em TOC (Psiq.)", "T");
            AddItemToDatabase("Sacerdote que conduz as cerimônias judaicas", "RABINO");
            AddItemToDatabase("Astatínio (símbolo)", "AT");
            AddItemToDatabase("Divide em dois", "DESDOBRA");
            AddItemToDatabase("Piratas a serviço oficial de seu país", "CORSARIOS");
            AddItemToDatabase("Bife fino de porco", "CARRE");
            AddItemToDatabase("(?)-língua, jogo verbal como \"Bagre branco, branco bagre\"", "TRAVA");
            AddItemToDatabase("Visconde de (?), economista do império", "CAIRU");
            AddItemToDatabase("Adorno feito com o produto das ostras", "COLARDEPEROLAS");
            AddItemToDatabase("Lois (?), a amada do Super-Homen (HQ)", "LANE");
            AddItemToDatabase("Alagoas (sigla)", "AL");
            AddItemToDatabase("Deter; reter", "ATER");
            AddItemToDatabase("Nascida no 1º signo do Zodíaco", "ARIANA");
            AddItemToDatabase("Cédula", "NOTA");
            AddItemToDatabase("A criança que recebeu a dose da Sabin", "VACINADA");
            AddItemToDatabase("Função do biombo em um ambiente", "ANTEPARO");
            AddItemToDatabase("Personagem de \"O Guarani\" (Lit.)", "CECI");
            AddItemToDatabase("Cerveja inglesa de cor clara", "ALE");
            AddItemToDatabase("\"Quem (?) consente\" (dito)", "CALA");
            AddItemToDatabase("República Árabe do Egito (sígla)", "RAE");
            AddItemToDatabase("(?) o queixo: tremer de frio", "BATER");
            AddItemToDatabase("Mandar (?): agir com disposição", "VER");
            AddItemToDatabase("Silvio Santos, fundador do SBT", "SS");
            AddItemToDatabase("Pedro (?), ex-apresentador do \"BBB\"", "BIAL");
            AddItemToDatabase("Carro, na linguagem infantil", "BIBI");
            //AddItemToDatabase("", "");
        }

        public void AddItemToDatabase(string question, string answer)
        {
            database.Add(new CrosswordDatabaseItem(question, answer));
        }

        public CrosswordDatabaseItem GetRandomItem()
        {
            return database.GetRandomElement();
        }

        public List<CrosswordDatabaseItem> GetRandomItems(int minCharCount, int maxCharCount, List<Tuple<int, string>> intersectionsTuples)
        {
            var list = database.FindAll(x => x.answer.Length >= minCharCount && x.answer.Length <= maxCharCount);

            for (int i = 0; i < intersectionsTuples.Count; i++)
            {
                list = list.FindAll(delegate (CrosswordDatabaseItem x)
                {
                    if (x.answer.Length <= intersectionsTuples[i].value1)
                        return true;
                    return x.answer[intersectionsTuples[i].value1].ToString() == intersectionsTuples[i].value2;
                });
            }
            return list;
        }

        public CrosswordDatabaseItem GetItem(int charcount, char firstchar)
        {
            return database.FindAll(x => x.AnswerCharCount(charcount)).FindAll(x => x.AnswerStartsWith(firstchar)).GetRandomElement();
        }
    }
}