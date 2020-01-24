import ServiceResponse from "interfaces/service-response";
import BaseServices from "services/base-service";

const baseEndpoint = "topics";

const get = async (): Promise<ServiceResponse<boolean>> => {
    return await BaseServices.get(baseEndpoint);
}

const TopicsServices = {
    get,
};

export default TopicsServices;