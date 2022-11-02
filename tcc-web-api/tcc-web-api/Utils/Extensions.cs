using tcc_web_api.Models.Enums;

namespace tcc_web_api.Utils {
    public static class Extensions {

        public static string GetString(this OccurrenceType type) {
            switch(type) {
                case OccurrenceType.Bug: return "Bug";
                case OccurrenceType.GameDesignProblem: return "Problema em Game Design";
                case OccurrenceType.DocumentationIssue: return "Problema na Documentação";
                case OccurrenceType.PrototypeInconsistency: return "Problema com os Protótipos";
                case OccurrenceType.TechProblem: return "Problema Técnico";
                case OccurrenceType.Testing: return "Testes Ineficazes";
                case OccurrenceType.DevToolsProblem: return "Problema com as Ferramentas de Desenvolvimento";
                case OccurrenceType.MissCommunication: return "Problema de Comunicação";
                case OccurrenceType.CrunchTime: return "Crunch (Trabalho Excessivo)";
                case OccurrenceType.TeamConflict: return "Conflitos na Equipe";
                case OccurrenceType.RemovedFeature: return "Remoção de Funcionalidade";
                case OccurrenceType.UnexpectedFeature: return "Nova Funcionalidade de Fora do Planejamento";
                case OccurrenceType.LowBudget: return "Problema Financeiros";
                case OccurrenceType.MissPlaning: return "Problema no Planejamento e Gestão do Projeto";
                case OccurrenceType.SecurityProblem: return "Problema de Segurança";
                case OccurrenceType.PoorBuiltScope: return "Problema com o Escopo do Projetos";
                case OccurrenceType.MarketingIssue: return "Problema com o Marketing";
                case OccurrenceType.MonetizationProblem: return "Problema no Planejamento de Monetização";
                default: return "Tipo Inválido";
            }
        }
    }
}
