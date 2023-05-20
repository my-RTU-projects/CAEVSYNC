import FilterAltIcon from '@mui/icons-material/FilterAlt';
import TransformIcon from '@mui/icons-material/Transform';
import ExpandIcon from '@mui/icons-material/Expand';
import {EventTransformationType} from "../models/EventTransformationType";

export const renderStepIcon = (eventTransformationType: EventTransformationType) => {
    if (eventTransformationType === EventTransformationType.FILTER)
        return <FilterAltIcon />;
    if (eventTransformationType === EventTransformationType.REPLACE)
        return <TransformIcon/>;
    if (eventTransformationType === EventTransformationType.EXPAND_TIME_RANGE)
        return <ExpandIcon/>;

    return null;
}