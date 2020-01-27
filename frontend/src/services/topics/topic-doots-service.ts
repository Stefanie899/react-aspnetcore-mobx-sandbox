import ServiceResponse from "interfaces/service-response";
import BaseServices from "services/base-service";
import TopicDoot from "interfaces/topics/topic-doot";

const endpoint = "topics/doot";

const post = async (doot: TopicDoot): Promise<ServiceResponse<TopicDoot>> => {
    return await BaseServices.post(endpoint, doot);
}

const put = async (doot: TopicDoot): Promise<ServiceResponse<TopicDoot>> => {
    return await BaseServices.put(endpoint, doot);
}

const get = async (id: number): Promise<ServiceResponse<TopicDoot>> => {
    return await BaseServices.get(`${endpoint}/${id}`);
}

const index = async (): Promise<ServiceResponse<TopicDoot[]>> => {
    return await BaseServices.get(endpoint, { userId: 1 });
}

const TopicDootsService = {
    get,
    index,
    post,
    put,
};

export default TopicDootsService;