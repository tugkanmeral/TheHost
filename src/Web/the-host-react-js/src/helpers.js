export function parseJwt(_token) {
    try {
        var base64Url = _token.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        return JSON.parse(jsonPayload)
    } catch (error) {
        console.log(error)
        return {}
    }
}

export function isObjNullOrEmpty(_obj) {
    // return _obj
    //     && Object.keys(_obj).length === 0
    //     && Object.getPrototypeOf(_obj) === Object.prototype

    return _obj === null || Object.keys(_obj).length === 0
}

export function getTokenObj() {
    let appToken = getToken();
    if (appToken)
        return parseJwt(appToken);
    else
        return null;
}

export function getToken() {
    return localStorage.getItem('appToken');
}