using System.Collections.Generic;
using System.Linq;
using Base.Entity;
using Base.Util.Matchers;

namespace Base.Util {
    public class Matcher {
        private readonly GameMatcher[] _matchers;

        private Matcher(GameMatcher[] matchers) {
            _matchers = matchers;
        }

        public IEntity[] Match(IEnumerable<IEntity> entities) {
            return entities.Where(entity => { return _matchers.All(matcher => matcher.Match(entity)); }).ToArray();
        }

        public static Matcher AllOf(GameMatcher[] matchers) {
            return new Matcher(matchers);
        }
    }
}