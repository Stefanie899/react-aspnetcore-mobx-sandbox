import React from "react";
import TopicDootsStore from "stores/topic-doots-store";

export const TopicDootsContext = React.createContext({
    topicDootsStore: new TopicDootsStore(),
});
