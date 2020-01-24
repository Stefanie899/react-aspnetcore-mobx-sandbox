import { CollectionUtils } from "utilities/collection-utils";

/*
---------------------------------------------------------------------------------------------
Private Methods
---------------------------------------------------------------------------------------------
*/

const _numericEnumToPojo = (enumObject: any): {} => {
    let pojo: { [k: string]: any } = {};

    for (const key in enumObject) {
        if (isNaN(parseInt(key))) {
            pojo[key] = enumObject[key];
        }
    }

    return pojo;
};

const _objectToArray = (object: any): any[] => {
    const result: any[] = [];

    if (CollectionUtils.isEmpty(object)) {
        return result;
    }

    for (const key in object) {
        if (object.hasOwnProperty(key)) {
            result.push(object[key]);
        }
    }

    return result;
};

/*
---------------------------------------------------------------------------------------------
Exports
---------------------------------------------------------------------------------------------
*/

export const CoreUtils = {
    numericEnumToPojo: _numericEnumToPojo,
    objectToArray: _objectToArray,
};
