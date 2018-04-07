class InferenceTest {
    matches(nodeContent) {
        throw new TypeError('This method should be overridden by inheriting classes.');
    }
    get parsedValue() {
        throw new TypeError('This method should be overridden by inheriting classes.');
    }
}

class IsEmpty extends InferenceTest {
    matches(nodeContent) {
        if (nodeContent && nodeContent.length) {
            return false;
        }
        return true;
    }
    get parsedValue() {
        return 0;
    }
}

class IsANumber extends InferenceTest {
    constructor() {
        // [BIB]:  https://stackoverflow.com/questions/31067368/javascript-es6-class-extend-without-super
        super();
        this._parsedValue = 0;
    }
    get parsedValue() {
        return this._parsedValue;
    }
    /* [BIB]:  https://stackoverflow.com/questions/9716468/is-there-any-function-like-isnumeric-in-javascript-to-validate-numbers */
    static isNumeric(n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    }
    matches(nodeContent) {
        if (nodeContent && nodeContent.length) {
            this._parsedValue = parseFloat(nodeContent);
            return IsANumber.isNumeric(nodeContent);
        }
        return false;
    }
}

class HasNumber extends InferenceTest {
    constructor() {
        super();
        this._parsedValue = 0;
    }
    get parsedValue() {
        return this._parsedValue;
    }
    matches(nodeContent) {
        if (nodeContent && nodeContent.length) {
            this._parsedValue = parseFloat(nodeContent);
            return !isNaN(parseFloat(nodeContent));
        }
        return false;
    }
}


class ValueTransform {
    transform(parsedValue) {
        throw new TypeError('This method should be overridden by inheriting classes.');
    }
}

class BugId extends ValueTransform {
    constructor() {
        super();
    }
    transform(parsedValue, curNode) {
        if (curNode.innerHTML.length > 5) {
            var autoLink = "https://bugzilla.mozilla.org/show_bug.cgi?id=" + parsedValue;
            var anchor = document.createElement("a");
            anchor.href = anchor.title = autoLink;
            anchor.innerHTML = "" + parsedValue;
            // [BIB]:  https://stackoverflow.com/questions/843680/how-to-replace-dom-element-in-place-using-javascript
            //curNode.parentNode.replaceChild(anchor, curNode);
            curNode.innerHTML = anchor.outerHTML;
        }
        return parsedValue;
    }
}


class InferEngine {
    static inferFromPage(query) {
        let inferer = new InferEngine();
        inferer.parseAllNodes(query);
    }
    static getNodeList(query) {
        console.debug("query is \"" + query + "\"");
        return document.querySelectorAll(query);
    }
    parseAllNodes(query) {
        let nodes = InferEngine.getNodeList(query);
        if (nodes.length < 1) {
            return;
        }
        // [BIB]:  https://stackoverflow.com/questions/1144705/best-way-to-store-a-key-value-array-in-javascript
        let mapTests = new Map(), mapTransforms = new Map();
        //mapTests.set("IsEmpty", new IsEmpty());
        mapTests.set("IsANumber", new IsANumber());
        //mapTests.set("HasNumber", new HasNumber());
        //mapTransforms.set("IsEmpty", new IsEmpty());
        mapTransforms.set("IsANumber", new BugId());
        //mapTransforms.set("HasNumber", new HasNumber());
        for (let i = 0, len = nodes.length; i < len; i++) {
            let curNode = nodes[i];
            console.debug("Node [" + i + "]" + curNode.innerHTML);
            for (var key of mapTests.keys()) {
                if (mapTests.get(key).matches(curNode.innerHTML)) {
                    mapTransforms.get(key).transform(mapTests.get(key).parsedValue, curNode);
                }
            }
        }
    }
}
