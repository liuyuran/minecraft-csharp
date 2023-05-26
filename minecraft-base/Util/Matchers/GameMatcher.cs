using System;
using Base.Entity;

namespace Base.Util.Matchers {
    public class GameMatcher {
        private readonly Func<IEntity, bool> rule;

        public GameMatcher(Func<IEntity, bool> rule) {
            this.rule = rule;
        }

        public bool Match(IEntity entity) {
            return rule.Invoke(entity);
        }

        public static GameMatcher withID(string id) {
            return new GameMatcher(entity => entity.ID == id);
        }
    }
}