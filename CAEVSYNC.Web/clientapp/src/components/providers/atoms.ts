import { atom } from "jotai";
import {ConnectedAccountModel} from "../../models/ConnectedAccountModel";
import {CalendarSelectModel} from "../../models/CalendarSelectModel";
import {SyncRuleListModel} from "../../models/SyncRuleListModel";
import { SyncRuleModel } from "../../models/SyncRuleModel";
import {CalendarRenderModel} from "../../models/CalendarRenderModel";
import {EventTransformationType} from "../../models/EventTransformationType";

export const newSyncRuleModalOpenAtom = atom<boolean>(false);

export const newSyncRuleTransformStepModalOpenAtom = atom<boolean>(false);

//

export const connectedAccountListAtom = atom<Array<ConnectedAccountModel>>([]);

export const calendarListAtom = atom<Array<CalendarSelectModel>>([]);

export const calendarListForSelectAtom = atom<Array<CalendarSelectModel>>([]);

export const syncRuleListAtom = atom<Array<SyncRuleListModel>>([]);

export const selectedAccountIdAtom = atom<string | undefined>(undefined);

export const selectedCalendarIdAtom = atom<string | undefined>(undefined);

export const selectedSyncRuleIdAtom = atom<number | undefined>(undefined);

export const selectedSyncRuleAtom = atom<SyncRuleModel | undefined>(undefined);

export const sourceCalendarRenderModelAtom = atom<CalendarRenderModel | undefined>(undefined);

export const targetCalendarRenderModelAtom = atom<CalendarRenderModel | undefined>(undefined);

export const selectedEdgeIdAtom = atom<string | undefined>(undefined);

export const newStepNodeTypeAtom = atom<EventTransformationType | undefined>(undefined);

export const newStepNodeCoordinatesAtom = atom<{x: number, y: number} | undefined>(undefined);

export const isSyncJobActiveAtom = atom<boolean>(false);