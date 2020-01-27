import React from "react";
import TopicDootsStore from "interfaces/stores/topic-doots-store";

export const TopicDootsContext = React.createContext({
    topicDootsStore: new TopicDootsStore(),
});
