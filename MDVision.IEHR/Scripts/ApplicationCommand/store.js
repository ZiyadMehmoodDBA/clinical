

var expires = { expires: 1000 * 60 * 60 * 24 * 365 };  // one day
var store = {
    sizeLimit: 5 * 1024 * 1000,
    clear: function (key, setName) {
        //Repository.bag        
        if (!setName)
            return amplify.store(key, null);
        else
            return this.clearSession(setName.concat('_set_', key));
    },
    clearAllBySetName: function (setName) {
        SetsList = {};
        setName = setName.concat('_set_');
        var storage = this.fetchAllSessionKeys(), key;
        for (key in storage) {
            var i = key.indexOf(setName);
            if (i > -1) {
                this.clearSession(key);
            }
        }
    },
    fetch: function (key, setName) {

        if (setName) {
            return this.fetchSession(setName.concat('_set_', key));
        }
        else {
            var val = amplify.store(key);
            if (val) {
                //return JSON.parse(val);
                return val;
            }
            else {
                return null;
            }
        }
    },

    save: function (key, value, setName) {

        if (setName) {
            this.saveSession(setName.concat('_set_', key), value);
        }
        else {
            //var val = JSON.stringify(value);
            var val = value;
            try {
                amplify.store(key, val, expires);
            } catch (ex) {
                console.log(ex);
            }

        }
        this.getMemorySize(this.sizeLimit);
    },
    saveSession: function (key, value) {
        if (!amplify.store.types.sessionStorage) {
            this.saveMemory(key, value);
        }
        else {
            //var val = JSON.stringify(value);
            var val = value;
            try {
                amplify.store.sessionStorage(key, val);
            }
            catch (ex) {
                this.saveMemory(key, value);
                console.log(ex);
            }

        }
        this.getMemorySize(this.sizeLimit);
    },
    fetchSession: function (key) {
        if (!amplify.store.types.sessionStorage) {
            return this.fetch(key);
        }
        else {
            var val = amplify.store.sessionStorage(key);
            if (val) {
                //return JSON.parse(val);
                return val;
            }
            val = this.fetchMemory(key);
            if (val) {
                return val;
            }
            else {
                return null;
            }
        }
    },
    clearSession: function (key) {
        if (!amplify.store.types.sessionStorage) {
            {
                return this.clearMemory(key);
            }
        } else {
            amplify.store.sessionStorage(key, null);
            return this.clearMemory(key);
        }
    },
    fetchAllKeys: function () {
        return amplify.store();
    },
    fetchAllSessionKeys: function () {
        return amplify.store.sessionStorage();
    },
    fetchAllMemoryKeys: function () {
        return amplify.store['memory']();
    },
    clearAll: function () {

        var storage = this.fetchAllKeys(), key;
        for (key in storage) {
            this.clear(key);
        }
    },
    clearAllSession: function () {

        var storage = this.fetchAllSessionKeys(), key;
        for (key in storage) {
            this.clearSession(key);
        }
        storage = this.fetchAllMemoryKeys();
        for (key in storage) {
            this.clearMemory(key);
        }

    },
    fetchAllSetName: function () {
        var SetsList = [];
        var storage = this.fetchAllSessionKeys(), key;
        for (key in storage) {
            var i = key.indexOf('_set_');
            if (i > -1) {
                SetsList.push(key.substring(0, i));
            }
        }

        storage = this.fetchAllMemoryKeys();
        for (key in storage) {
            var i = key.indexOf('_set_');
            if (i > -1) {
                SetsList.push(key.substring(0, i));
            }
        }

        SetsList = $.grep(SetsList, function (v, k) {
            return $.inArray(v, SetsList) === k;
        });

        return SetsList;
    },
    fetchAllBySetName: function (setName) {
        SetsList = {};
        setName = setName.concat('_set_');
        var storage = this.fetchAllSessionKeys(), key;
        for (key in storage) {
            var i = key.indexOf(setName);
            if (i > -1) {
                SetsList[key.substring(setName.length, key.length)] = this.fetch(key);
            }
        }

        storage = this.fetchAllMemoryKeys();
        for (key in storage) {
            var i = key.indexOf(setName);
            if (i > -1) {
                SetsList[key.substring(setName.length, key.length)] = this.fetch(key);
            }
        }

        return SetsList;
    },
    clearMemory: function (key) {
        //Repository.bag        
        return amplify.store['memory'](key, null);
    },
    fetchMemory: function (key) {

        var val = amplify.store['memory'](key);
        return val || null;
        //if (val) {
        //    return JSON.parse(val);
        //}
        //else {
        //    return null;
        //}
    },
    saveMemory: function (key, value) {


        //var val = JSON.stringify(value);
        amplify.store['memory'](key, value);
    },

    getMemorySize: function (limit) {

        var data = "";
        var length = 0;
        var storage = this.fetchAllKeys();
        for (key in storage) {
            data = this.fetch(key);
            length = length + data.length;
        }

        storage = this.fetchAllMemoryKeys();
        for (key in storage) {
            data = this.fetchMemory(key);
            length = length + data.length;
        }

        storage = this.fetchAllSessionKeys();
        for (key in storage) {
            data = this.fetchSession(key);
            length = length + data.length;
        }
        if (length > limit) {
            console.log("User has reached maximum limit");
        }
    }
};
//sessionSave = function (key, value) {    //leaving it for future implementation
//    if (amplify.store.types.sessionStorage) {
//        amplify.store.sessionStorage(key, value);
//    }
//}
