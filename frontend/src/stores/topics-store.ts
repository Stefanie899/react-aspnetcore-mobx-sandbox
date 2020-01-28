import { observable, action } from 'mobx'
import Topic from "interfaces/topics/topic";
import TopicsServices from "services/topics/topics-service";

export class TopicsStore {
    @observable
    topics: Topic[] = [];

    @action
    setTopics(topics: Topic[]) {
        this.topics = topics;
    }

    @action
    setTopic(topic: Topic) {
        const topicIndex = this.topics.findIndex(e => e.id === topic.id)

        if (topicIndex >= 0) {
            this.topics[topicIndex] = topic;
        } else {
            this.topics.push(topic);
        }
    }

    @action
    async getTopics() {
        const response = await TopicsServices.index();
        const result = response.resultObject;

        if (response.errors != null && response.errors.length > 0) {
            console.log("There was an error!");
            console.debug(response.errors);

            return response;
        }

        console.debug(this);
        console.debug(result);

        this.setTopics(result);

        return response;
    }

    @action
    async getTopic(id: number) {
        const response = await TopicsServices.get(id);
        const result = response.resultObject;

        if (response.errors != null && response.errors.length > 0) {
            console.log("There was an error!");
            console.debug(response.errors);

            return response;
        }

        console.debug(this);
        console.debug(result);

        if (result != null) {
            this.setTopic(result);
        }

        return response;
    }
}

export default TopicsStore;