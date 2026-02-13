namespace FiscalizAI.Core.Common.Bases;

public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTime DataCadastro { get; private set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            DataCadastro = DateTime.UtcNow;
        }

        //Comparação por Identidade, não por Referência
        public override bool Equals(object? obj)
        {
            var compareTo = obj as BaseEntity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (compareTo is null) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(BaseEntity a, BaseEntity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.Equals(b);
        }

        public static bool operator !=(BaseEntity a, BaseEntity b) => !(a == b);

        public override int GetHashCode() => (GetType().GetHashCode() * 907) + Id.GetHashCode();

        public override string ToString() => $"{GetType().Name} [Id={Id}]";
    }
