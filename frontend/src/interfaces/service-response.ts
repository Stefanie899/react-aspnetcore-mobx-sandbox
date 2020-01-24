import Error from "interfaces/error";

interface ServiceResponse<T> {
    nextLinkParams: string;
    errors:         Error[];
    resultObject:   T;
    errorCount:     number;
    hasErrors:      boolean;
}

export default ServiceResponse;