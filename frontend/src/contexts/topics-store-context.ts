import React from "react";
import TopicsStore from "stores/topics-store";

export const TopicsContext = React.createContext({
    topicsStore: new TopicsStore(),
});
