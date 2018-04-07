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

class IsADate extends InferenceTest {
    constructor() {
        super();
        this._parsedValue = new Date();
    }
    get parsedValue() {
        return this._parsedValue;
    }
    /* [BIB]:  https://stackoverflow.com/questions/7445328/check-if-a-string-is-a-date-value */
    static isDate(date) {
        return (date !== "Invalid Date" && !isNaN(date)) ? true : false;
    }
    matches(nodeContent) {
        if (nodeContent && nodeContent.length) {
            this._parsedValue = new Date(nodeContent);
            return IsADate.isDate(this._parsedValue);
        }
        return false;
    }
}

class IsEmail extends InferenceTest {
    constructor() {
        super();
        this._parsedValue = [];
    }
    matches(nodeContent) {
        if (nodeContent && nodeContent.length) {
            // [BIB]:  https://stackoverflow.com/a/16424719
            var emailsArray = nodeContent.match(/([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\.[a-zA-Z0-9._-]+)/gi);
            if (emailsArray != null && emailsArray.length) {
                this._parsedValue = emailsArray;
                return true;
            }
        }
        return false;
    }
    get parsedValue() {
        return this._parsedValue;
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

class RelativeDateInfo extends ValueTransform {
    constructor() {
        super();
        RelativeDateInfo._AVG_DAYS_PER_YEAR = 365.25;
        RelativeDateInfo._AVG_DAYS_PER_MONTH = 30.4375;
        RelativeDateInfo._MS_PER_DAY = 1000 * 60 * 60 * 24;
    }
    // [BIB]:  https://stackoverflow.com/questions/3224834/get-difference-between-2-dates-in-javascript
    static dateDiffInDays(a, b) {
        // Discard the time and time-zone information.
        let utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
        let utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
        return Math.floor((utc2 - utc1) / RelativeDateInfo._MS_PER_DAY);
    }
    static daysToRelativeTimeSpan(diffInDays) {
        var absDiffInDays = Math.abs(diffInDays);
        if (absDiffInDays < 1) {
            return "today";
        }
        let years = Math.floor(absDiffInDays / RelativeDateInfo._AVG_DAYS_PER_YEAR),
            residue = Math.floor(absDiffInDays % RelativeDateInfo._AVG_DAYS_PER_YEAR),
            months = Math.floor(residue / RelativeDateInfo._AVG_DAYS_PER_MONTH),
            days = Math.floor(absDiffInDays % RelativeDateInfo._AVG_DAYS_PER_MONTH),
            relativeTimeSpan = "~ ";
        if (years > 0) {
            if (years == 1) {
                relativeTimeSpan = "1 year, ";
            }
            else {
                relativeTimeSpan = years + "years, ";
            }
        }
        if (months > 0) {
            if (months == 1) {
                relativeTimeSpan += "1 month, ";
            }
            else {
                relativeTimeSpan += months + " months, ";
            }
        }
        if (days > 0) {
            if (days == 1) {
                relativeTimeSpan += "1 day ";
            }
            else {
                relativeTimeSpan += days + " days ";
            }
        }
        if (diffInDays > 0) {
            relativeTimeSpan += "ago.";
        }
        else {
            relativeTimeSpan += "from today.";
        }
        return relativeTimeSpan;
    }
    transform(parsedValue, curNode) {
        var diff = RelativeDateInfo.dateDiffInDays(parsedValue, new Date());
        var relativeTimeSpan = RelativeDateInfo.daysToRelativeTimeSpan(diff);
        curNode.setAttribute("title", relativeTimeSpan);
    }
}

class EmailAutoLinkify extends ValueTransform {
    constructor() {
        super();
    }
    transform(parsedValue, curNode) {
        let newContent = curNode.innerHTML;
        if (parsedValue.length > 0) {
            for (var i = 0, len = parsedValue.length; i < len; i++) {
                let email = parsedValue[i], anchor = "<a href=mailto:" + email + " title=mailto:\"" + email + "\">" + email + "</a>";
                newContent = newContent.replace(email, anchor);
            }
            curNode.innerHTML = newContent;
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
        mapTests.set("IsADate", new IsADate());
        mapTests.set("IsEmail", new IsEmail());
        //mapTransforms.set("IsEmpty", new IsEmpty());
        mapTransforms.set("IsANumber", new BugId());
        //mapTransforms.set("HasNumber", new HasNumber());
        mapTransforms.set("IsADate", new RelativeDateInfo());
        mapTransforms.set("IsEmail", new EmailAutoLinkify());
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
