using System.Collections.Generic;

namespace OGC.EvidenceSystem
{
    public interface EvidenceOwner { }
    public interface Evidence // 證物
    {
        Evidence TransferOwnership(EvidenceOwner receiver);
        EvidenceOwner GetOwner();
        ICollection<Fact> GetFactsRevealable(FactOwner factOwner);
    }
    public interface FactOwner 
    {
        T AddFact<T>(ICollection<Fact> facts) where T : FactOwner;
        ICollection<Fact> GetFacts();
    }
    public interface Fact { } // 事實
    public static class Rules
    {
        public static T Reveal<T>(Evidence evidence, T factOwner)
        where T : FactOwner
        {
            ICollection<Fact> facts = evidence.GetFactsRevealable(factOwner);
            factOwner = factOwner.AddFact<T>(facts);
            return factOwner;
        }
        public static (Evidence evidence, T receiver) TransferEvidence<T>(Evidence evidence, T receiver)
        where T : EvidenceOwner, FactOwner
        {
            receiver = Reveal<T>(evidence, receiver);
            evidence = evidence.TransferOwnership(receiver);
            return (evidence, receiver);
        }
    }
}
