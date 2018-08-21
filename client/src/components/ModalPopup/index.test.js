import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import ModalPopup from './index';

it('renders correctly', () => {
    const component = shallow(<ModalPopup onHide={() => {}} />);

    expect(component).toMatchSnapshot();
});