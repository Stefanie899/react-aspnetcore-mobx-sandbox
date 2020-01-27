import ServiceResponse from "interfaces/service-response";
import BaseServices from "services/base-service";
import Topic from "interfaces/topics/topic";

const baseEndpoint = "topics";

const index = async (): Promise<ServiceResponse<Topic[]>> => {
    return await BaseServices.get(baseEndpoint);
}

const get = async (id: number): Promise<ServiceResponse<Topic>> => {
    return await BaseServices.get(`${baseEndpoint}/${id}`);
}

const TopicsServices = {
    index,
    get,
};

export default TopicsServices;