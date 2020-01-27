import { observable, action } from 'mobx'
import TopicDoot from 'interfaces/topics/topic-doot';
import TopicDootsService from 'services/topics/topic-doots-service';

export class TopicDootsStore {
    @observable
    topicDoots: TopicDoot[] = [];

    @action
    setTopicDoots(topicDoots: TopicDoot[]) {
        this.topicDoots = topicDoots;
    }

    @action
    setTopicDoot(topicDoot: TopicDoot) {
        const topicDootIndex = this.topicDoots.findIndex(e => e.id === topicDoot.id)

        if (topicDootIndex >= 0) {
            this.topicDoots[topicDootIndex] = topicDoot;
        } else {
            this.topicDoots.push(topicDoot);
        }
    }

    @action
    async addOrUpdateDoot(doot: TopicDoot) {
        if (doot.id == null) {
            return this.addDoot(doot);
        } else {
            return this.updateDoot(doot);
        }
    }

    @action
    async addDoot(doot: TopicDoot) {
        const response = await TopicDootsService.post(doot);
        const result = response.resultObject;

        if (response.errors != null && response.errors.length > 0) {
            console.log("There was an error!");
            console.debug(response.errors);

            return response;
        }

        if (result.id != null) {
            this.getTopicDoot(result.id);
        }

        return response;
    }

    @action
    async updateDoot(doot: TopicDoot) {
        const response = await TopicDootsService.put(doot);
        const result = response.resultObject;

        if (response.errors != null && response.errors.length > 0) {
            console.log("There was an error!");
            console.debug(response.errors);

            return response;
        }

        if (result.id != null) {
            this.getTopicDoot(result.id);
        }

        return response;
    }

    @action
    async getTopicDoots() {
        const response = await TopicDootsService.index();
        const result = response.resultObject;

        if (response.errors != null && response.errors.length > 0) {
            console.log("There was an error!");
            console.debug(response.errors);

            return response;
        }

        this.setTopicDoots(result);

        return response;
    }

    @action
    async getTopicDoot(id: number) {
        const response = await TopicDootsService.get(id);
        const result = response.resultObject;

        if (response.errors != null && response.errors.length > 0) {
            console.log("There was an error!");
            console.debug(response.errors);

            return response; 
        }

        if (result != null) {
            this.setTopicDoot(result);
        }

        return response;
    }
}

export default TopicDootsStore;