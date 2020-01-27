import axios from "axios";
import ServiceResponse from "interfaces/service-response";

const baseEndpoint = "https://localhost:5001/api/v1/";

const authHeader = () => {
    // return authorization header with jwt token
    const userString = localStorage.getItem("user");

    if (userString == null || userString === "undefined") {
        return {};
    }

    let user = JSON.parse(userString);

    if (user != null && user.token != null) {
        return { 'Authorization': 'Bearer ' + user.token };
    } else {
        return {};
    }
}

const get = async <T>(endpoint: string, params?: any): Promise<ServiceResponse<T>> => {
    const headers = authHeader();

    return await axios.get(getUrl(baseEndpoint + endpoint, params),
        {
            headers: headers
        })
        .then((r) => {
            return r.data;
        })
        .catch((r) => {
            return {
                errors: [r]
            };
        })
}

const post = async (endpoint: string, data: any) => {
    const headers = Object.assign({
        "Accept": "application/json",
        "Content-Type": "application/json",
    }, authHeader());

    return await axios.post(baseEndpoint + endpoint, data,
        {
            headers: headers
        })
        .then((r) => {
            return r.data;
        })
        .catch((r) => {
            return {
                errors: [r]
            };
        })
}

const put = async (endpoint: string, data: any) => {
    const headers = Object.assign({
        "Accept": "application/json",
        "Content-Type": "application/json",
    }, authHeader());

    return await axios.put(baseEndpoint + endpoint, data,
        {
            headers: headers
        })
        .then((r) => {
            return r.data;
        })
        .catch((r) => {
            return {
                errors: [r]
            };
        })
}

const getUrl = (fullEndpoint: string, params ?: any, payload ?: any, endpointSuffix ?: string): string => {
    let url = fullEndpoint;
    const pattern = /(?:\{(.*?)\})/g;
    let matches;
    const dictionary: any[] = [];

    if (endpointSuffix) {
        const separator = url.endsWith("/") ? "" : "/";
        url = `${url}${separator}${endpointSuffix}`;
    }

    // find all path params in the url
    do {
        matches = pattern.exec(url);
        if (matches) {
            dictionary.push(matches[1]);
        }
    } while (matches);

    // add params to the url
    if (params) {

        // replace all path params first
        for (const p of dictionary) {
            if (params.hasOwnProperty(p)
                && params[p] != null) {
                const value = params[p];
                url = url.replace("\{" + p + "\}", value);
                delete params[p];
                delete dictionary[p];
            }
        }

        // add all remaining as querystring params
        let paramCount = 0;
        let queryParams = "";
        for (const p in params) {
            if (params.hasOwnProperty(p)
                && params[p] != null) {
                const paramIsArray = Array.isArray(params[p]);
                queryParams += (paramCount > 0 ? "&" : "");
                if (!paramIsArray) {
                    queryParams += p + "=" + params[p];
                }
                else {
                    queryParams += p + "=" + params[p].join("&" + p + "=");
                }
                paramCount += 1;
            }
        }
        if (queryParams.length > 0) {
            url += `?${queryParams}`;
        }
    }

    // replace any path params if there's a match on the payload
    if (payload) {
        for (const p of dictionary) {
            if (payload.hasOwnProperty(p)
                && payload[p]) {
                const value = payload[p];
                url = url.replace("\{" + p + "\}", value);
            }
        }
    }

    // clear out the remaining, unmatched parameters
    url = url.replace(/\{.*?\}/g, "");

    return url;
};

const BaseServices = {
    get,
    post,
    put,
};

export default BaseServices;