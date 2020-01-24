import ServiceResponse from "interfaces/service-response";
import BaseServices from "services/base-service";
import Topic from "interfaces/topics/topic";

const baseEndpoint = "topics";

const get = async (): Promise<ServiceResponse<Topic[]>> => {
    return await BaseServices.get(baseEndpoint);
}

const TopicsServices = {
    get,
};

export default TopicsServices;